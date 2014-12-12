﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Project_Novi.Api;
using Project_Novi.Text;

namespace Project_Novi.Render
{
    /// <summary>
    /// Implements an animated avatar that can talk.
    /// </summary>
    public class Avatar
    {
        private const int Width = 560;
        private const int Height = 640;
        private readonly IController _controller;
        private int _talkingCounter;

        public enum Animated
        {
            Mouth,
            Pupils,
            LeftEye,
            RightEye,
            Nose
        }

        private int _blinkDelay;
        private readonly Random _rand;

        private TTS _tts;

        private readonly List<Bitmap> _leftEyePinch = new List<Bitmap>()
        {
            Properties.Resources.left_eye_blink1,
            Properties.Resources.left_eye_pinch,
            Properties.Resources.left_eye_pinch,
            Properties.Resources.left_eye_blink1
        };

        private readonly List<Bitmap> _rightEyePinch = new List<Bitmap>()
        {
            Properties.Resources.right_eye_blink1,
            Properties.Resources.right_eye_pinch,
            Properties.Resources.right_eye_pinch,
            Properties.Resources.right_eye_blink1
        };

        private readonly List<Bitmap> _pupilsPinch = new List<Bitmap> {
            Properties.Resources.pupils,
            Properties.Resources.blank,
            Properties.Resources.blank,
            Properties.Resources.pupils
        };

        public string Saying { get; set; }

        /// <summary>
        /// An animation for the blinking of the left eye.
        /// </summary>
        private readonly List<Bitmap> _leftEyeBlink = new List<Bitmap> {
            Properties.Resources.left_eye_blink1,
            Properties.Resources.left_eye_blink2,
            Properties.Resources.left_eye_blink3,
            Properties.Resources.left_eye_blink3,
            Properties.Resources.left_eye_blink2,
            Properties.Resources.left_eye_blink1
        };

        /// <summary>
        /// An animation for the blinking of the right eye.
        /// </summary>
        private readonly List<Bitmap> _rightEyeBlink = new List<Bitmap> {
            Properties.Resources.right_eye_blink1,
            Properties.Resources.right_eye_blink2,
            Properties.Resources.right_eye_blink3,
            Properties.Resources.right_eye_blink3,
            Properties.Resources.right_eye_blink2,
            Properties.Resources.right_eye_blink1
        };

        /// <summary>
        /// An animation for the blinking of the pupils.
        /// </summary>
        private readonly List<Bitmap> _pupilsBlink = new List<Bitmap> {
            Properties.Resources.pupils,
            Properties.Resources.pupils,
            Properties.Resources.blank,
            Properties.Resources.blank,
            Properties.Resources.pupils,
            Properties.Resources.pupils
        };

        /// <summary>
        /// A collection of animations for the different animated parts of the face.
        /// Every value is a list of points along which the element will be animated.
        /// </summary>
        private readonly Dictionary<Animated, List<Point>> _offsetAnimations = new Dictionary<Animated, List<Point>>
        {
            { Animated.Mouth, new List<Point>() },
            { Animated.Pupils, new List<Point>() },
            { Animated.LeftEye, new List<Point>() },
            { Animated.RightEye, new List<Point>() },
            { Animated.Nose, new List<Point>() }
        };

        /// <summary>
        /// A collection of animations for the different animated parts of the face.
        /// Every value is a list images which the element will be displayed as for one frame.
        /// </summary>
        private readonly Dictionary<Animated, List<Bitmap>> _bitmapAnimations = new Dictionary<Animated, List<Bitmap>>
        {
            { Animated.Mouth, new List<Bitmap>() },
            { Animated.Pupils, new List<Bitmap>() },
            { Animated.LeftEye, new List<Bitmap>() },
            { Animated.RightEye, new List<Bitmap>() },
            { Animated.Nose, new List<Bitmap>() }
        };

        public Avatar(IController controller)
        {
            _controller = controller;
            _rand = new Random();
        }

        internal void Attach()
        {
            _controller.Tick += ControllerOnTick;
            if (_tts != null) _tts.Talking = false;
        }

        private void ControllerOnTick()
        {
            foreach (var kv in _offsetAnimations.Where(kv => kv.Value.Count > 0))
                kv.Value.RemoveAt(0);
            foreach (var kv in _bitmapAnimations.Where(kv => kv.Value.Count > 0))
                kv.Value.RemoveAt(0);

            // Blink after a random amount of ticks.
            if (_blinkDelay > 0)
            {
                _blinkDelay--;
            }
            else
            {
                Blink();
                _blinkDelay = _rand.Next(60) + 20;
            }

            // While the avatar is still talking keep adding the talking animation.
            if (_tts.Talking)
            {
                if (_talkingCounter == 0)
                {
                    var images = new[]
                    {
                        Properties.Resources.open_happy,
                        Properties.Resources.open_round,
                        Properties.Resources.closed_happy,
                        Properties.Resources.open_round
                    };
                    Animate(Animated.Mouth, images, 5);
                    _talkingCounter = 20;
                }
                else
                {
                    _talkingCounter--;
                }
            }
        }

        /// <summary>
        /// Add the offsets to the animation path of the part of the face.
        /// </summary>
        public void Animate(Animated animated, IEnumerable<Point> offsets)
        {
            _offsetAnimations[animated].AddRange(offsets);
        }

        /// <summary>
        /// Animate a part of the face from a set of coordinates to another set of coordinates,
        /// moving one pixel in each direction every tick.
        /// </summary>
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

        /// <summary>
        /// Add the bitmaps to the animation path of the part of the face.
        /// </summary>
        public void Animate(Animated animated, IEnumerable<Bitmap> bitmaps)
        {
            _bitmapAnimations[animated].AddRange(bitmaps);
        }

        /// <summary>
        /// Add a collection of bitmaps to the animation path of a part of the face,
        /// but display every bitmap for a certain amount of ticks.
        /// </summary>
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

        /// <summary>
        /// Let the avatar say something, animating her mouth along the way.
        /// </summary>
        /// <param name="text">The text to speak.</param>
        public void Say(string text)
        {
            Saying = text;
            if (_tts != null)
                _tts.Talking = false;

            _tts = new TTS();
            _tts.TextToSpeech(text, () =>
            {
                _bitmapAnimations[Animated.Mouth].Clear();
            });
        }

        /// <summary>
        /// Make the avatar's eyes blink.
        /// </summary>
        public void Blink()
        {
            Animate(Animated.RightEye, _rightEyeBlink);
            Animate(Animated.LeftEye, _leftEyeBlink);
            Animate(Animated.Pupils, _pupilsBlink);
        }

        public void Pinch()
        {
            Animate(Animated.LeftEye, _leftEyePinch);
            Animate(Animated.RightEye, _rightEyePinch);
            Animate(Animated.Pupils, _pupilsPinch);
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
            // Every part of the face should be scaled to make it look right.
            var scale = Math.Min(rectangle.Width / (float)Width,
                                 rectangle.Height / (float)Height);
            var pupilOffset = GetAnimationOffset(Animated.Pupils);
            var faceOffset = GetAnimationOffset(Animated.LeftEye);

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

            graphics.DrawImage(Properties.Resources.based, avatarRect);
            graphics.DrawImage(Properties.Resources.vneck_green, avatarRect);
            graphics.DrawImage(GetAnimationBitmap(Animated.LeftEye, Properties.Resources.left_eye_open), faceRect);
            graphics.DrawImage(GetAnimationBitmap(Animated.RightEye, Properties.Resources.right_eye_open), faceRect);
            graphics.DrawImage(GetAnimationBitmap(Animated.Pupils, Properties.Resources.pupils), pupilRect);
            graphics.DrawImage(GetAnimationBitmap(Animated.Nose, Properties.Resources.nose), faceRect);
            graphics.DrawImage(GetAnimationBitmap(Animated.Mouth, Properties.Resources.closed_happy), faceRect);
        }
    }
}