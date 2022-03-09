using UnityEngine;
using System.Collections;
using Antura.Minigames;

namespace Antura.Minigames.Scanner
{
    public class ScannerDevice : MonoBehaviour
    {

        public float minX;
        public float maxX;

        bool isDragging = false;
        private Vector3 screenPoint;
        private Vector3 offset;

        public GameObject goLight;

        public Vector3 fingerOffset;

        public float backDepth = -5f;

        public float depthMovementSpeed = 10f;

        public ScannerGame game;

        public float smoothedDraggingSpeed;

        //private float timeDelta;

        private float frontDepth;

        private bool moveBack = false;

        private bool letterEventsNotSet = true;

        float dragDamping = 50;

        void Start()
        {
            goLight.SetActive(false);

            //timeDelta = 0;
            frontDepth = transform.position.z;

        }

        void OnLetterFlying(ScannerLivingLetter sender)
        {
            moveBack = true;
            StopAllCoroutines();
            StartCoroutine(co_Reset());
        }

        public float speed = 1;
        public ScannerLivingLetter LL;
        Minigames.IAudioSource wordSound;


        void Update()
        {

            calculateSmoothedSpeed();

            if (game.scannerLL.Count != 0 && letterEventsNotSet)
            {
                letterEventsNotSet = false;
                foreach (ScannerLivingLetter LL in game.scannerLL)
                {
                    LL.onFlying += OnLetterFlying;
                }
            }

            if (moveBack && transform.position.z < backDepth)
            {
                transform.Translate(Vector3.forward * depthMovementSpeed * Time.deltaTime);
            }
            else if (!moveBack && transform.position.z > frontDepth)
            {
                transform.Translate(Vector3.back * depthMovementSpeed * Time.deltaTime);
            }


            if (dataAudio != null)
            {
                if (curPos != prevPose)
                {
                    playTime = Mathf.Clamp(playTime + Time.deltaTime, 0, dataAudio.Duration);
                    prevPose = scanPos;
                    scanPos = Mathf.Lerp(0, dataAudio.Duration, Vector3.Distance(scanStartPos, transform.position) / 15f);

                    /*if (Mathf.Abs(scanPos) < Mathf.Abs(prevPose))
                        dataAudio.Pitch = -Mathf.Clamp(1 + (scanPos - playTime), 0, 100);
                    else*/
                    dataAudio.Pitch = Mathf.Clamp(1 + (scanPos - playTime), 0, 100);

                    //Debug.LogError(dataAudio.Position + " / " + dataAudio.Duration);
                    //Debug.LogError(Mathf.Abs(scanPos) + " / " + Mathf.Abs(prevPose));
                }
                else
                {
                    dataAudio.Pitch = 0;
                    /*scanStartPos = transform.position;
                    playTime = scanPos;*/
                }
                if (!isDragging || (willPronounce && (scanPos >= dataAudio.Duration - 0.1f /*|| dataAudio.Position >= dataAudio.Duration - 0.1f*/)))
                {
                    dataAudio = null;
                    willPronounce = false;
                    LL.setColor(Color.white);
                }
            }

            if (willPronounce && LL && Vector3.Distance(LL.collForScan.position, transform.position) > 5.5f)
            {
                dataAudio = null;
                willPronounce = false;
                LL.setColor(Color.white);

            }

        }


        float curPos, prevPose;
        float[] values = new float[16];
        int i = 0;
        void calculateSmoothedSpeed()
        {
            prevPose = curPos;
            curPos = Mathf.Abs(transform.position.x);

            values[i] = Mathf.Abs(curPos - prevPose);
            i++;
            if (i == values.Length)
                i = 0;

            for (int x = 0; x < values.Length; x++)
                smoothedDraggingSpeed = smoothedDraggingSpeed + values[x];

            smoothedDraggingSpeed = (smoothedDraggingSpeed / values.Length);

            //draggingSpeed = Mathf.SmoothDamp( draggingSpeed, Mathf.Abs(curPos - prevPose), ref draggingSpeed, Time.deltaTime);// time;
            //draggingSpeed = (draggingSpeed + prevPose) / 2;
            //Debug.LogError(smoothedDraggingSpeed);
        }

        public void Reset()
        {
            moveBack = false;
        }

        IEnumerator co_Reset()
        {
            yield return new WaitForSeconds(3.25f);
            moveBack = false;
        }

        void OnMouseDown()
        {
            if (game.disableInput)
            {
                return;
            }

            isDragging = true;
            goLight.SetActive(true);
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position -
                Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        }

        void OnMouseDrag()
        {
            if (isDragging)
            {
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
                transform.position = Vector3.Lerp(transform.position, new Vector3(
                    Mathf.Clamp(curPosition.x, minX, maxX),
                    transform.position.y,
                    transform.position.z), Time.deltaTime * dragDamping);


            }

        }

        void OnMouseUp()
        {
            isDragging = false;
            goLight.SetActive(false);
            Reset();
        }

        //string lastTag = "";
        Vector3 scanStartPos;
        float playTime, scanPos;
        public bool willPronounce = false;
        IAudioSource dataAudio;

        void OnTriggerStay(Collider other)
        {
            if ((other.tag == ScannerGame.TAG_SCAN_START || other.tag == ScannerGame.TAG_SCAN_END) && isDragging)
            {
                if (!willPronounce)
                {
                    scanStartPos = transform.position;
                    playTime = 0;
                    willPronounce = true;

                    if (LL)
                        LL.setColor(Color.white);

                    LL = other.transform.parent.GetComponent<ScannerLivingLetter>();
                    dataAudio = game.Context.GetAudioManager().PlayVocabularyData(LL.LLController.Data, true);

                    LL.setColor(Color.green);

                    if (game.tut.tutStep == 1)
                        game.tut.setupTutorial(2, LL);

                }
                /*if (timeDelta == 0 || lastTag == other.tag)
				{
					timeDelta = Time.time;
					lastTag = other.tag;
				}
				else
				{
					ScannerLivingLetter LL = other.transform.parent.GetComponent<ScannerLivingLetter>();
					timeDelta = Time.time - timeDelta;
					game.PlayWord(timeDelta, LL);
					timeDelta = 0;

                    if(game.tut.tutStep == 1)
                        game.tut.setupTutorial(2, LL);
                }*/
            }


            if (other.gameObject.name.Equals("Antura") && isDragging)
            {
                game.antura.GetComponent<ScannerAntura>().beScared();
            }
        }


    }
}
