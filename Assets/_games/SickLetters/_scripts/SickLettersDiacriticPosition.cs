using UnityEngine;


namespace Antura.Minigames.SickLetters
{
    public class SickLettersDiacriticPosition : MonoBehaviour
    {

        public MeshRenderer letterMesh;
        public MeshRenderer dotmesh;
        public float offSetX = 0.0f;
        public float offSetY = 0.0f;

        private MeshRenderer diacriticMesh;

        void Awake()
        {
            diacriticMesh = GetComponent<MeshRenderer>();
        }

        public void Hide()
        {
            //diacriticText.color = AppManager.I.SetAlpha(diacriticText.color,0);
        }

        public void Show()
        {
            //diacriticText.color = AppManager.I.SetAlpha(diacriticText.color,DancingDotsGameManager.instance.dotHintAlpha);
        }

        public void CheckPosition()
        {
            if (letterMesh && diacriticMesh)
            {
                float newY = Mathf.Clamp(diacriticMesh.bounds.extents.y, 0.5f, 5f) + offSetY;

                /*if (false) // TODO: diacritic == Diacritic.Kasrah
				{
					float letterBottom = letterMesh.bounds.center.y - letterMesh.bounds.extents.y;
					float dotBottom = dotmesh.bounds.center.y - dotmesh.bounds.extents.y;
					newY = -newY;
					newY += letterBottom < dotBottom ? letterBottom : dotBottom;
				}
				else
				{
					float letterTop = letterMesh.bounds.center.y + letterMesh.bounds.extents.y;
					float dotTop = dotmesh.bounds.center.y + dotmesh.bounds.extents.y;
					newY += letterTop > dotTop ? letterTop : dotTop;
				}	*/

                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
        }

    }
}
