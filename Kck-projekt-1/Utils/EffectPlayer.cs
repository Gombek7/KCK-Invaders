using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekt_1.Utils
{
    class EffectPlayer
    {
        class Effect
        {
            public int x, y, frame;
            public bool finished = false;
            Art art;
            public Effect(int x, int y, Art art)
            {
                this.x = x;
                this.y = y;
                this.frame = -1;
                this.art = art;
            }

            public void NextFrame()
            {
                frame++;
                finished = frame == art.Skins;
                if (finished)
                    ConsoleUtils.Fill(' ',x, y, x + art.Width, y + art.Height);
                else
                    art.Draw(x, y, frame);
            }
        }

        List<Effect> effects;

        public int Count { get => effects.Count; }

        public EffectPlayer()
        {
            effects = new List<Effect>();
        }

        public void PlayEffect(int x, int y, Art art)
        {
            effects.Add(new Effect(x, y, art));
        }

        public void UpdateEffects()
        {
            foreach (Effect effect in effects)
                effect.NextFrame();
            effects.RemoveAll(effect => effect.finished);
        }

        public void Clear()
        {
            effects.Clear();
        }
    }
}
