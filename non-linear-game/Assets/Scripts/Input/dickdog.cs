using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Input {
    using global::Input;

    using UniRx;
    using UniRx.Triggers;

    using UnityEngine;
    public class dickdog : MonoBehaviour {
        protected void Start() {
            StringReactiveProperty derp = new StringReactiveProperty();
            derp.AsObservable();

            var asdf = this.GetComponent<Collider>().OnMouseEnterAsObservable();
            var fdsa = this.GetComponent<Collider>().OnMouseExitAsObservable();
            var poop = new OnMouseEnterHandler(this.GetComponent<SpriteRenderer>());
            var poop2 = new OnMouseExitHandler(this.GetComponent<SpriteRenderer>());
            poop.Subscribe(asdf);
            fdsa.Subscribe(poop2);

        }
    }
}
