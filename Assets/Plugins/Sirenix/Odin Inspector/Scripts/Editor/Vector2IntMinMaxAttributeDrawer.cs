//-----------------------------------------------------------------------
// <copyright file="Vector2IntMinMaxAttributeDrawer.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Reflection;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR && UNITY_2017_2_OR_NEWER

namespace Sirenix.OdinInspector.Editor.Drawers
{
    /// <summary>
    ///     Draws Vector2Int properties marked with <see cref="MinMaxSliderAttribute" />.
    /// </summary>
    public class Vector2IntMinMaxAttributeDrawer : OdinAttributeDrawer<MinMaxSliderAttribute, Vector2Int>
    {
        private string errorMessage;
        private InspectorPropertyValueGetter<float> floatMaxGetter;
        private InspectorPropertyValueGetter<float> floatMinGetter;

        private InspectorPropertyValueGetter<int> intMaxGetter;

        private InspectorPropertyValueGetter<int> intMinGetter;

        private InspectorPropertyValueGetter<Vector2Int> vector2IntMinMaxGetter;

        /// <summary>
        ///     Initializes the drawer by resolving any optional references to members for min/max value.
        /// </summary>
        protected override void Initialize()
        {
            MemberInfo member;

            // Min member reference.
            if (Attribute.MinMember != null)
                if (MemberFinder.Start(Property.ParentType)
                    .IsNamed(Attribute.MinMember)
                    .HasNoParameters()
                    .TryGetMember(out member, out errorMessage))
                {
                    var type = member.GetReturnType();
                    if (type == typeof(int))
                        intMinGetter = new InspectorPropertyValueGetter<int>(Property, Attribute.MinMember);
                    else if (type == typeof(float))
                        floatMinGetter = new InspectorPropertyValueGetter<float>(Property, Attribute.MinMember);
                }

            // Max member reference.
            if (Attribute.MaxMember != null)
                if (MemberFinder.Start(Property.ParentType)
                    .IsNamed(Attribute.MaxMember)
                    .HasNoParameters()
                    .TryGetMember(out member, out errorMessage))
                {
                    var type = member.GetReturnType();
                    if (type == typeof(int))
                        intMaxGetter = new InspectorPropertyValueGetter<int>(Property, Attribute.MaxMember);
                    else if (type == typeof(float))
                        floatMaxGetter = new InspectorPropertyValueGetter<float>(Property, Attribute.MaxMember);
                }

            // Min max member reference.
            if (Attribute.MinMaxMember != null)
            {
                vector2IntMinMaxGetter = new InspectorPropertyValueGetter<Vector2Int>(Property, Attribute.MinMaxMember);
                if (errorMessage != null) errorMessage = vector2IntMinMaxGetter.ErrorMessage;
            }
        }

        /// <summary>
        ///     Draws the property.
        /// </summary>
        protected override void DrawPropertyLayout(GUIContent label)
        {
            // Get the range of the slider from the attribute or from member references.
            Vector2 range;
            if (vector2IntMinMaxGetter != null && errorMessage == null)
            {
                range = vector2IntMinMaxGetter.GetValue();
            }
            else
            {
                if (intMinGetter != null)
                    range.x = intMinGetter.GetValue();
                else if (floatMinGetter != null)
                    range.x = floatMinGetter.GetValue();
                else
                    range.x = Attribute.MinValue;

                if (intMaxGetter != null)
                    range.y = intMaxGetter.GetValue();
                else if (floatMaxGetter != null)
                    range.y = floatMaxGetter.GetValue();
                else
                    range.y = Attribute.MaxValue;
            }

            // Display evt. error message.
            if (errorMessage != null) SirenixEditorGUI.ErrorMessageBox(errorMessage);

            EditorGUI.BeginChangeCheck();
            var value = SirenixEditorFields.MinMaxSlider(label, ValueEntry.SmartValue, range, Attribute.ShowFields);
            if (EditorGUI.EndChangeCheck()) ValueEntry.SmartValue = new Vector2Int((int) value.x, (int) value.y);
        }
    }
}
#endif // UNITY_EDITOR && UNITY_2017_2_OR_NEWER