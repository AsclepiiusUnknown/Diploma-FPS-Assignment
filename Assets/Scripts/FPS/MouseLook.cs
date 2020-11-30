using System;
using Rewired;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class MouseLook
    {
        public float xSensitivity = 2f;
        public float ySensitivity = 2f;
        public float scopingMultiplier = .5f;
        public bool clampVerticalRotation = true;
        public float minX = -90F;
        public float maxX = 90F;
        public bool smooth;
        public float smoothTime = 5f;
        public bool lockCursor = true;
        private Quaternion camTargetRot;

        private Quaternion charTargetRot;
        private bool cursorIsLocked = true;
        public static bool _isScoping;

        [HideInInspector]
        public Player _player;

        public void Init(Transform character, Transform camera)
        {
            charTargetRot = character.localRotation;
            camTargetRot = camera.localRotation;
        }

        public void LookRotation(Transform character, Transform camera)
        {
            Vector2 input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Vector2 reInput = new Vector2(_player.GetAxis("Mouse X"), _player.GetAxis("Mouse Y"));
            var yRot = (input.x + reInput.x) * xSensitivity;
            var xRot = (input.y + reInput.y) * ySensitivity;

            if (_isScoping)
            {
                xRot *= scopingMultiplier;
                yRot *= scopingMultiplier;
            }

            charTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            camTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
                camTargetRot = ClampRotationAroundXAxis(camTargetRot);

            if (smooth)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, charTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, camTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = charTargetRot;
                camera.localRotation = camTargetRot;
            }

            UpdateCursorLock();
        }

        public void SetCursorLock(bool value)
        {
            lockCursor = value;
            //we force unlock the cursor if the user disable the cursor locking helper
            Cursor.lockState = (lockCursor) ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !lockCursor;
        }

        public void UpdateCursorLock()
        {
            //if the user set "lockCursor" we check & properly lock the cursos
            if (lockCursor)
                InternalLockUpdate();
        }

        private void InternalLockUpdate()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
                cursorIsLocked = false;
            else if (Input.GetMouseButtonUp(0)) cursorIsLocked = true;

            if (cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            var angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, minX, maxX);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }
    }
}