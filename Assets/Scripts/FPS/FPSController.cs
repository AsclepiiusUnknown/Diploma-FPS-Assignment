using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class FPSController : MonoBehaviour
    {
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            jumping = false;
            audioSource = GetComponent<AudioSource>();
            m_MouseLook.Init(transform, m_Camera.transform);
        }


        // Update is called once per frame
        private void Update()
        {
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!jump) jump = Input.GetButtonDown("Jump");

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                moveDir.y = 0f;
                jumping = false;
            }

            if (!m_CharacterController.isGrounded && !jumping && m_PreviouslyGrounded) moveDir.y = 0f;

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
        }


        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            var desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            moveDir.x = desiredMove.x * speed;
            moveDir.z = desiredMove.z * speed;


            if (m_CharacterController.isGrounded)
            {
                moveDir.y = -stickToGroundForce;

                if (jump)
                {
                    moveDir.y = jumpSpeed;
                    PlayJumpSound();
                    jump = false;
                    jumping = true;
                }
            }
            else
            {
                moveDir += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
            }

            if (!m_CharacterController.isGrounded && !canAirWalk)
            {
                moveDir.x = m_CharacterController.velocity.x;
                moveDir.z = m_CharacterController.velocity.z;
            }

            m_CollisionFlags = m_CharacterController.Move(moveDir * Time.fixedDeltaTime);

            if (Input.GetButtonUp("Jump"))
                if (moveDir.y > 0)
                    moveDir.y *= jumpCut;

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            var body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below) return;

            if (body == null || body.isKinematic) return;
            body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }


        private void PlayLandingSound()
        {
            audioSource.clip = landSound;
            audioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void PlayJumpSound()
        {
            audioSource.clip = jumpSound;
            audioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
                m_StepCycle += (m_CharacterController.velocity.magnitude + speed * (isWalking ? 1f : runStepLengthen)) *
                               Time.fixedDeltaTime;

            if (!(m_StepCycle > m_NextStep)) return;

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded) return;
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            var n = Random.Range(1, footstepSounds.Length);
            audioSource.clip = footstepSounds[n];
            audioSource.PlayOneShot(audioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            footstepSounds[n] = footstepSounds[0];
            footstepSounds[0] = audioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob) return;
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                        speed * (isWalking ? 1f : runStepLengthen));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }

            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            // Read input
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var waswalking = isWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            isWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            speed = isWalking ? walkSpeed : runSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1) m_Input.Normalize();

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (isWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!isWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView()
        {
            if (m_MouseLook == null)
            {
                print("**NULL**");
                return;
            }

            m_MouseLook.LookRotation(transform, m_Camera.transform);
        }

        #region |VARIABLES

        #region ||Movement

        [Header("Movement")] public float walkSpeed;

        public float runSpeed;
        public float jumpSpeed;
        public float jumpCut;
        public bool canAirWalk;

        [Range(0f, 1f)] public float runStepLengthen;

        //*PRIVATE//
        private bool isWalking;
        private bool jump;
        private bool jumping;
        private Vector2 m_Input;
        private Vector3 moveDir = Vector3.zero;

        #endregion

        #region ||Mouse

        [Header("Mouse")] public MouseLook m_MouseLook;

        //*PRIVATE//
        private Camera m_Camera;

        #endregion

        #region ||Gravity

        [Header("Gravity")] public float gravityMultiplier;

        public float stickToGroundForce;

        #endregion

        #region ||Audio

        [Header("Audio")] public AudioClip jumpSound; // the sound played when character leaves the ground.

        public AudioClip landSound; // the sound played when character touches back on ground.
        public AudioClip[] footstepSounds; // an array of footstep sounds that will be randomly selected from.

        //*PRIVATE//
        private AudioSource audioSource;

        #endregion

        #region ||Effects

        [Header("Effects")] public bool m_UseFovKick;

        public FOVKick m_FovKick = new FOVKick();
        public bool m_UseHeadBob;
        public CurveControlledBob m_HeadBob = new CurveControlledBob();
        public LerpControlledBob m_JumpBob = new LerpControlledBob();
        public float m_StepInterval;

        #endregion

        #region ||Misc

        [Header("Misc.")]
        //*PRIVATE//
        private float m_YRotation;

        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;

        #endregion

        #endregion
    }
}