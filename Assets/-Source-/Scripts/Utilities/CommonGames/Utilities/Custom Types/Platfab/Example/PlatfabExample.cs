using UnityEngine;

namespace CommonGames.Utilities.CustomTypes.Examples
{
    using CommonGames.Utilities.CustomTypes;
    
    public class PlatfabExample : MonoBehaviour
    {
        [Sirenix.OdinInspector.InlineEditorAttribute]
        [SerializeField] private Platfab _platfab = null;

        [SerializeField] private KeyCode spawnKey = KeyCode.Space;

        private void Update()
        {
            if(!Input.GetKeyDown(spawnKey)) return;

            Vector3 __pos = CommonGames.Utilities.CGTK.CGRandom.PointsOnUnitSphere(1, 5)[0];

            _platfab.Instantiate(
                position: __pos, 
                rotation: Quaternion.identity);
        }
    }
}