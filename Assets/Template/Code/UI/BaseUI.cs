using Framework;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class BaseUI : BaseMono<UIManager>
    {

        public virtual void Awake()
        {
            mgr = UIManager.Instance;
            GetChildElement();
        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void GetChildElement()
        {

        }

        public virtual void InitElement()
        {

        }

        public virtual void EnterAnim()
        {
            InitElement();
        }

        public virtual float ExitAnim()
        {
            return 0;
        }

        public virtual void OnEnter()
        {
            gameObject.SetActive(true);
            EnterAnim();
        }

        public virtual void OnExit()
        {
            StartCoroutine(Hide(ExitAnim()));
        }

        IEnumerator Hide(float delay)
        {
            yield return new WaitForSeconds(delay);
            gameObject.SetActive(false);
        }

    }
}