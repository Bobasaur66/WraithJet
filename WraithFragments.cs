using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WraithJet
{
    public class WraithFragments
    {
        public static void RegisterFragments()
        {
            CustomPrefab wraithFragment = new CustomPrefab("wraithFragment", "Wraith Fragment", "Fragment for a submersible aircraft called the Wraith");
            CloneTemplate gameObject = new CloneTemplate(wraithFragment.Info, "1f5cee66-a02f-4693-a1bd-928c938c7e77");
            wraithFragment.SetGameObject(gameObject);

            wraithFragment.CreateFragment(TechType.Seamoth, 6f, 4, null, true, true);

            wraithFragment.Register();
        }
    }
}
