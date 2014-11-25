﻿using System.Drawing;

namespace Project_Novi.Modules.Home
{
    class HomeView : IView
    {
        private readonly HomeModule _module;
      
        public IModule Module
        {
            get { return _module; }
        }

        public HomeView(HomeModule module)
        {
            _module = module;
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            graphics.Clear(Color.FromArgb(255, 32, 103, 178));

            var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            Rectangle rect1 = new Rectangle(1, 1, 1920, 300);
            Font strFont = new Font("Sergoe UI", 50);
            //Available categories: Welkom, Poke, Idle, Kaart, RouteVragen, RouteBerekenen en BerekendeRoute
            //Let op: Exact overnemen!!!
            var strTxt = Text.TextManager.GetText("Welkom");

            TTS.TTS.TextToSpeech(strTxt);
            graphics.DrawString(strTxt, strFont, Brushes.White, rect1, stringFormat);


            // Draw avatar
            int avatarStartX = 680;
            int avatarStartY = 440;
            int faceOffsetX = avatarStartX + 165;
            int faceOffsetY = avatarStartY + 190;
            int pupilOffsetX = faceOffsetX + 33;
            int pupilOffsetY = faceOffsetY + 33;

            Rectangle avatarRect = new Rectangle(avatarStartX, avatarStartY, 560, 640);
            Rectangle faceRect = new Rectangle(faceOffsetX, faceOffsetY, 224, 158);
            Rectangle pupilRect= new Rectangle(pupilOffsetX, pupilOffsetY, 160, 18);

            graphics.DrawImage(Properties.Resources.avatar_base, avatarRect);
            graphics.DrawImage(Properties.Resources.avatar_eyes, faceRect);
            graphics.DrawImage(Properties.Resources.avatar_pupils, pupilRect);
            graphics.DrawImage(Properties.Resources.avatar_nose, faceRect);
            graphics.DrawImage(Properties.Resources.closed_happy, faceRect);
        }
    }
}
