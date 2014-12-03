using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Project_Novi.Render
{
    class Avatar
    {
        private const int Width = 560;
        private const int Height = 640;
        private IController _controller;

        public enum Animated
        {
            Mouth,
            Pupils,
            Eyes,
            Nose
        }

        private readonly Dictionary<Animated, List<Point>> _offsetAnimations = new Dictionary<Animated, List<Point>>
        {
            { Animated.Mouth, new List<Point>() },
            { Animated.Pupils, new List<Point>() },
            { Animated.Eyes, new List<Point>() },
            { Animated.Nose, new List<Point>() }
        };

        private readonly Dictionary<Animated, List<Bitmap>> _bitmapAnimations = new Dictionary<Animated, List<Bitmap>>
        {
            { Animated.Mouth, new List<Bitmap>() },
            { Animated.Pupils, new List<Bitmap>() },
            { Animated.Eyes, new List<Bitmap>() },
            { Animated.Nose, new List<Bitmap>() }
        };

        public Avatar(IController controller)
        {
            _controller = controller;
            _controller.Tick += ControllerOnTick;
        }

        private void ControllerOnTick()
        {
            foreach (var kv in _offsetAnimations.Where(kv => kv.Value.Count > 0))
                kv.Value.RemoveAt(0);
            foreach (var kv in _bitmapAnimations.Where(kv => kv.Value.Count > 0))
                kv.Value.RemoveAt(0);
        }

        public void Animate(Animated animated, IEnumerable<Point> offsets)
        {
            _offsetAnimations[animated].AddRange(offsets);
        }

        public void Animate(Animated animated, int fromX, int fromY, int toX, int toY)
        {
            var xStep = fromX <= toX ? 1 : -1;
            var yStep = fromY <= toY ? 1 : -1;
            var x = fromX;
            var y = fromY;
            while (x != toX || y != toY)
            {
                _offsetAnimations[animated].Add(new Point(x, y));
                if (x != toX) x += xStep;
                if (y != toY) y += yStep;
            }
        }

        public void Animate(Animated animated, IEnumerable<Bitmap> bitmaps)
        {
            _bitmapAnimations[animated].AddRange(bitmaps);
        }

        public void Animate(Animated animated, IEnumerable<Bitmap> bitmaps, int ticksPerFrame)
        {
            foreach (var b in bitmaps)
            {
                for (var i = 0; i < ticksPerFrame; i++)
                {
                    _bitmapAnimations[animated].Add(b);
                }
            }
        }

        private Point GetAnimationOffset(Animated animated)
        {
            return _offsetAnimations[animated].Count > 0 ? _offsetAnimations[animated][0] : new Point(0, 0);
        }

        private Bitmap GetAnimationBitmap(Animated animated, Bitmap def)
        {
            return _bitmapAnimations[animated].Count > 0 ? _bitmapAnimations[animated][0] : def;
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            var scale = Math.Min(rectangle.Width / (float)Width,
                                 rectangle.Height / (float)Height);
            var pupilOffset = GetAnimationOffset(Animated.Pupils);
            var faceOffset = GetAnimationOffset(Animated.Eyes);

            var faceOffsetX = (int)(165 * scale + faceOffset.X * scale);
            var faceOffsetY = (int)(190 * scale + faceOffset.Y * scale);
            var faceWidth = (int)(224 * scale);
            var faceHeight = (int)(158 * scale);

            var pupilOffsetX = faceOffsetX + (int)(33 * scale + pupilOffset.X * scale);
            var pupilOffsetY = faceOffsetY + (int)(33 * scale + pupilOffset.Y * scale);
            var pupilWidth = (int)(160 * scale);
            var pupilHeight = (int)(18 * scale);

            var offsetX = (int)(rectangle.X + rectangle.Width / 2 - (Width / 2) * scale);
            var offsetY = (int)(rectangle.Bottom - Height * scale);

            var avatarRect = new Rectangle(offsetX, offsetY, (int)(Width * scale), (int)(Height * scale));
            var faceRect = new Rectangle(offsetX + faceOffsetX, offsetY + faceOffsetY, faceWidth, faceHeight);
            var pupilRect = new Rectangle(offsetX + pupilOffsetX, offsetY + pupilOffsetY, pupilWidth, pupilHeight);

            graphics.DrawImage(Properties.Resources.avatar_base, avatarRect);
            graphics.DrawImage(GetAnimationBitmap(Animated.Eyes, Properties.Resources.avatar_eyes), faceRect);
            graphics.DrawImage(GetAnimationBitmap(Animated.Pupils, Properties.Resources.avatar_pupils), pupilRect);
            graphics.DrawImage(GetAnimationBitmap(Animated.Nose, Properties.Resources.avatar_nose), faceRect);
            graphics.DrawImage(GetAnimationBitmap(Animated.Mouth, Properties.Resources.closed_happy), faceRect);
        }
    }
}
