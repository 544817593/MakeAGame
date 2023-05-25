/****************************************************************************
 * 2023.5 DESKTOP-4BKRNF5
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace PackOpen
{
	public partial class Pack
	{
		[SerializeField] public UnityEngine.UI.Button BtnOpen;

		public void Clear()
		{
			BtnOpen = null;
		}

		public override string ComponentName
		{
			get { return "Pack";}
		}
	}
}
