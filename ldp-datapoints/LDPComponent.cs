﻿using Assets.Scripts.eca2ld_unity;
using Assets.Scripts.eca2ld_unity.ld_components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ECA2LD
{
    public class LDPComponent : LinkedDataPoint
    {
        private LDComponent c;
        private List<LDPAttribute> attributes = new List<LDPAttribute>();

        public LDPComponent(GameObject gameObject, Component c) : base(gameObject)
        {
            this.c = c as LDComponent;
            this.c.dataPoint = this;

            Name = c.GetType().ToString();
            string uri = gameObject.GetComponent<LDEntity>().Uri
                + c.GetType() + "/";
            LDPGraph = new ComponentLDPGraph(new Uri(uri), c);
            initializeHttpListener(uri);
            CreateAttributeDatapoints();
        }

        public override void Shutdown()
        {
            foreach (LDPAttribute a in attributes)
            {
                a.Shutdown();
            }

            base.Shutdown();
        }
        private void CreateAttributeDatapoints()
        {
            foreach (var f in c.GetType().GetFields())
            {
                var attr = (IsLDAttribute[])f.GetCustomAttributes(typeof(IsLDAttribute), false);
                if (attr.Length > 0)
                {
                    attributes.Add(new LDPAttribute(gameObject, new LDAttribute(c, f.Name, f.GetValue(c))));
                }
            }
        }
    }
}
