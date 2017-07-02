﻿#if HOVER_INPUT_VRTK

using System;
using Hover.Core.Cursors;
using UnityEngine;

namespace Hover.InputModules.VRTK {

	/*================================================================================================*/
	[Serializable]
	public class VRTKCursor {

		public static float IndexTriggerMax = 0.88f;
		public static float TouchpadMax = 0.8f;

		public enum InputSourceType {
			None,
			Trigger,
			TouchpadY,
			TouchpadUp,
			TouchpadDown,
			TouchpadX,
			TouchpadLeft,
			TouchpadRight,
			TouchpadTouch,
			TouchpadPress,
			GripPress,
			MenuPress
		}

		public CursorType Type { get; private set; }

		public Vector3 LocalPosition = Vector3.zero;
		public Vector3 LocalRotation = Vector3.zero;
		public InputSourceType TriggerStrengthInput = InputSourceType.Trigger;
		public InputSourceType CursorSizeInput = InputSourceType.None;

		[Range(0.01f, 0.1f)]
		public float MinSize = 0.01f;

		[Range(0.02f, 0.2f)]
		public float MaxSize = 0.03f;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public VRTKCursor(CursorType pType) {
			Type = pType;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateData(HoverCursorDataProvider pCursorDataProv,
																HoverInputVRTK.ControlState pState) {
			ICursorDataForInput data = GetData(pCursorDataProv);

			if ( data == null ) {
				return;
			}

			data.SetUsedByInput(true);

			UpdateDataWithLocalOffsets(data, pState);
			UpdateDataForTrigger(data, pState);
			UpdateDataForSize(data, pState);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateDataWithLocalOffsets(ICursorDataForInput pData, 
															HoverInputVRTK.ControlState pState) {
			Vector3 worldOffset = pState.Tx.TransformVector(LocalPosition);

			pData.SetWorldPosition(pState.Tx.position+worldOffset);
			pData.SetWorldRotation(pState.Tx.rotation*Quaternion.Euler(LocalRotation));
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateDataForTrigger(ICursorDataForInput pData,
															HoverInputVRTK.ControlState pState) {
			float prog = GetInputSourceProgress(TriggerStrengthInput, pState, 0);
			pData.SetTriggerStrength(prog);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateDataForSize(ICursorDataForInput pData,
															HoverInputVRTK.ControlState pState) {
			float prog = GetInputSourceProgress(CursorSizeInput, pState, 0.5f);
			pData.SetSize(Mathf.Lerp(MinSize, MaxSize, prog));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private ICursorDataForInput GetData(HoverCursorDataProvider pCursorDataProv) {
			if ( !pCursorDataProv.HasCursorData(Type) ) {
				return null;
			}

			return pCursorDataProv.GetCursorDataForInput(Type);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private float GetInputSourceProgress(InputSourceType pInputSourceType,
											HoverInputVRTK.ControlState pState, float pDefault) {
			switch ( pInputSourceType ) {
				case InputSourceType.Trigger:
					return Mathf.InverseLerp(0, IndexTriggerMax, pState.TriggerAxis);

				case InputSourceType.TouchpadY:
					return Mathf.InverseLerp(-TouchpadMax, TouchpadMax, pState.TouchpadAxis.y);

				case InputSourceType.TouchpadUp:
					return Mathf.InverseLerp(0, TouchpadMax, pState.TouchpadAxis.y);

				case InputSourceType.TouchpadDown:
					return Mathf.InverseLerp(0, -TouchpadMax, pState.TouchpadAxis.y);

				case InputSourceType.TouchpadX:
					return Mathf.InverseLerp(-TouchpadMax, TouchpadMax, pState.TouchpadAxis.x);

				case InputSourceType.TouchpadLeft:
					return Mathf.InverseLerp(0, -TouchpadMax, pState.TouchpadAxis.x);

				case InputSourceType.TouchpadRight:
					return Mathf.InverseLerp(0, TouchpadMax, pState.TouchpadAxis.x);

				case InputSourceType.TouchpadTouch:
					return (pState.TouchpadTouch ? 1 : 0);

				case InputSourceType.TouchpadPress:
					return (pState.TouchpadPress ? 1 : 0);

				case InputSourceType.GripPress:
					return (pState.GripPress ? 1 : 0);

				case InputSourceType.MenuPress:
					return (pState.MenuPress ? 1 : 0);
			}

			return pDefault;
		}

	}

}

#endif //HOVER_INPUT_VRTK
