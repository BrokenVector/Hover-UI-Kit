#if HOVER_INPUT_VRTK

using Hover.Core.Cursors;
using UnityEngine;
using VRTK;

namespace Hover.InputModules.VRTK
{
    /*================================================================================================*/

    [ExecuteInEditMode]
    public class HoverInputVRTK : MonoBehaviour
    {
        public struct ControlState
        {
            public VRTK_ControllerEvents Controller;
            public Transform Tx;
            public Vector2 TouchpadAxis;
            public float TriggerAxis;
            public bool TouchpadTouch;
            public bool TouchpadPress;
            public bool GripPress;
            public bool MenuPress;
        }

        public ControlState StateLeft { get; private set; }
        public ControlState StateRight { get; private set; }

        public HoverCursorDataProvider CursorDataProvider;
        public VRTK_ControllerEvents Left;
        public VRTK_ControllerEvents Right;

        [Space(12)] public FollowCursor Look = new FollowCursor(CursorType.Look);

        [Space(12)] public VRTKCursor LeftPalm = new VRTKCursor(CursorType.LeftPalm)
        {
            LocalPosition = new Vector3(0, 0.01f, 0),
            LocalRotation = new Vector3(90, 0, 0),
            CursorSizeInput = VRTKCursor.InputSourceType.TouchpadX,
            MinSize = 0.04f,
            MaxSize = 0.06f
        };

        public VRTKCursor LeftThumb = new VRTKCursor(CursorType.LeftThumb)
        {
            LocalPosition = new Vector3(0, 0, -0.17f),
            LocalRotation = new Vector3(-90, 0, 0)
        };

        public VRTKCursor LeftIndex = new VRTKCursor(CursorType.LeftIndex)
        {
            LocalPosition = new Vector3(-0.05f, 0, 0.03f),
            LocalRotation = new Vector3(90, -40, 0)
        };

        public VRTKCursor LeftMiddle = new VRTKCursor(CursorType.LeftMiddle)
        {
            LocalPosition = new Vector3(0, 0, 0.06f),
            LocalRotation = new Vector3(90, 0, 0)
        };

        public VRTKCursor LeftRing = new VRTKCursor(CursorType.LeftRing)
        {
            LocalPosition = new Vector3(0.05f, 0, 0.03f),
            LocalRotation = new Vector3(90, 40, 0)
        };

        public VRTKCursor LeftPinky = new VRTKCursor(CursorType.LeftPinky)
        {
            LocalPosition = new Vector3(0.08f, 0, -0.06f),
            LocalRotation = new Vector3(-90, -180, 80),
            TriggerStrengthInput = VRTKCursor.InputSourceType.TouchpadLeft //for Hovercast
        };

        [Space(12)] public VRTKCursor RightPalm = new VRTKCursor(CursorType.RightPalm)
        {
            LocalPosition = new Vector3(0, 0.01f, 0),
            LocalRotation = new Vector3(90, 0, 0),
            CursorSizeInput = VRTKCursor.InputSourceType.TouchpadX,
            MinSize = 0.04f,
            MaxSize = 0.06f
        };

        public VRTKCursor RightThumb = new VRTKCursor(CursorType.RightThumb)
        {
            LocalPosition = new Vector3(0, 0, -0.17f),
            LocalRotation = new Vector3(-90, 0, 0)
        };

        public VRTKCursor RightIndex = new VRTKCursor(CursorType.RightIndex)
        {
            LocalPosition = new Vector3(0.05f, 0, 0.03f),
            LocalRotation = new Vector3(90, 40, 0)
        };

        public VRTKCursor RightMiddle = new VRTKCursor(CursorType.RightMiddle)
        {
            LocalPosition = new Vector3(0, 0, 0.06f),
            LocalRotation = new Vector3(90, 0, 0)
        };

        public VRTKCursor RightRing = new VRTKCursor(CursorType.RightRing)
        {
            LocalPosition = new Vector3(-0.05f, 0, 0.03f),
            LocalRotation = new Vector3(90, -40, 0)
        };

        public VRTKCursor RightPinky = new VRTKCursor(CursorType.RightPinky)
        {
            LocalPosition = new Vector3(-0.08f, 0, -0.06f),
            LocalRotation = new Vector3(-90, 180, -80),
            TriggerStrengthInput = VRTKCursor.InputSourceType.TouchpadRight //for Hovercast
        };


        ////////////////////////////////////////////////////////////////////////////////////////////////
        /*--------------------------------------------------------------------------------------------*/

        public void Awake()
        {
            CursorUtil.FindCursorReference(this, ref CursorDataProvider, false);

            if (Look.FollowTransform == null)
            {
                Look.FollowTransform = Camera.main.transform;
            }
        }

        /*--------------------------------------------------------------------------------------------*/

        public void Update()
        {
            if (!CursorUtil.FindCursorReference(this, ref CursorDataProvider, true))
            {
                return;
            }

            if (!Application.isPlaying)
            {
                return;
            }

            CursorDataProvider.MarkAllCursorsUnused();
            UpdateCursorsWithControllers();
            Look.UpdateData(CursorDataProvider);
            CursorDataProvider.ActivateAllCursorsBasedOnUsage();
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////
        /*--------------------------------------------------------------------------------------------*/

        private void UpdateCursorsWithControllers()
        {
            StateLeft = GetControllerState(Left);
            StateRight = GetControllerState(Right);

            LeftPalm.UpdateData(CursorDataProvider, StateLeft);
            LeftThumb.UpdateData(CursorDataProvider, StateLeft);
            LeftIndex.UpdateData(CursorDataProvider, StateLeft);
            LeftMiddle.UpdateData(CursorDataProvider, StateLeft);
            LeftRing.UpdateData(CursorDataProvider, StateLeft);
            LeftPinky.UpdateData(CursorDataProvider, StateLeft);

            RightPalm.UpdateData(CursorDataProvider, StateRight);
            RightThumb.UpdateData(CursorDataProvider, StateRight);
            RightIndex.UpdateData(CursorDataProvider, StateRight);
            RightMiddle.UpdateData(CursorDataProvider, StateRight);
            RightRing.UpdateData(CursorDataProvider, StateRight);
            RightPinky.UpdateData(CursorDataProvider, StateRight);
        }

        /*--------------------------------------------------------------------------------------------*/

        private ControlState GetControllerState(VRTK_ControllerEvents control)
        {
            var state = new ControlState
            {
                Controller = control,
                Tx = control.transform,
                TouchpadAxis = control.GetTouchpadAxis(),
                TriggerAxis = control.GetTriggerAxis(),
                TouchpadTouch = control.touchpadTouched,
                TouchpadPress = control.touchpadPressed,
                GripPress = control.gripPressed,
                MenuPress = control.menuPressed
            };

            return state;
        }
    }
}

#else

using Hover.Core.Utils;

namespace Hover.InputModules.VRTK {

	/*================================================================================================*/
	public class HoverInputVRTK : HoverInputMissing {

		public override string ModuleName { get { return "VRTK"; } }
		public override string RequiredSymbol { get { return "HOVER_INPUT_VRTK"; } }

	}

}

#endif //HOVER_INPUT_VRTK