using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace PackOpen
{
    public class PackModel : Architecture<PackModel>
    {
        public static BindableProperty<bool> finish = new BindableProperty<bool>();
        protected override void Init()
        {
            finish.Value = false;
        }
        
    }
    

    
}
