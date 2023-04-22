/****************************************************************************
 * 2023.4 DESKTOP-4BKRNF5
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace DialogueUI
{
	public partial class Choice
	{
		[SerializeField] public UnityEngine.UI.Button Choice1;
		[SerializeField] public UnityEngine.UI.Button Choice2;
		[SerializeField] public UnityEngine.UI.Button Choice3;
		[SerializeField] public UnityEngine.UI.Button Choice4;
		[SerializeField] public UnityEngine.UI.Button Choice5;

		public void Clear()
		{
			Choice1 = null;
			Choice2 = null;
			Choice3 = null;
			Choice4 = null;
			Choice5 = null;
		}

		public override string ComponentName
		{
			get { return "Choice";}
		}
	}
}
