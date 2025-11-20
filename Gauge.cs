using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Xmrig_Ranch_Launcher
{
    public class Gauge : Control
    {
        private float _value = 0;
        private float _minValue = 0;
        private float _maxValue = 100;
        private Color _baseLineColor = Color.FromArgb(40, 40, 60);
        private Color _scaleColor = Color.SteelBlue;
        private Color _needleColor = Color.SteelBlue;

        public float Value
        {
            get => _value;
            set { _value = Math.Max(_minValue, Math.Min(_maxValue, value)); Invalidate(); }
        }

        public float MinValue
        {
            get => _minValue;
            set { _minValue = value; Invalidate(); }
        }

        public float MaxValue
        {
            get => _maxValue;
            set { _maxValue = value; Invalidate(); }
        }

        public Color BaseLineColor
        {
            get => _baseLineColor;
            set { _baseLineColor = value; Invalidate(); }
        }

        public Color ScaleColor
        {
            get => _scaleColor;
            set { _scaleColor = value; Invalidate(); }
        }

        public Color NeedleColor
        {
            get => _needleColor;
            set { _needleColor = value; Invalidate(); }
        }

        public Gauge()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | 
                     ControlStyles.UserPaint | 
                     ControlStyles.DoubleBuffer | 
                     ControlStyles.ResizeRedraw, true);
            Size = new Size(120, 120);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            
            // Draw background (base line)
            using (var bgBrush = new SolidBrush(_baseLineColor))
                g.FillEllipse(bgBrush, rect);

            // Draw gauge arc (scale)
            if (_maxValue > _minValue)
            {
                float sweepAngle = 270f * ((_value - _minValue) / (_maxValue - _minValue));
                using (var pen = new Pen(_scaleColor, 8))
                {
                    pen.EndCap = LineCap.Round;
                    g.DrawArc(pen, rect, 135, sweepAngle);
                }
            }

            // Draw border
            using (var borderPen = new Pen(Color.FromArgb(80, 80, 100), 2))
                g.DrawEllipse(borderPen, rect);

            // Draw center dot
            using (var centerBrush = new SolidBrush(_needleColor))
                g.FillEllipse(centerBrush, Width / 2 - 3, Height / 2 - 3, 6, 6);

            // Draw needle
            if (_maxValue > _minValue)
            {
                float angle = 135f + (270f * ((_value - _minValue) / (_maxValue - _minValue)));
                float rad = (float)(angle * Math.PI / 180);
                int centerX = Width / 2;
                int centerY = Height / 2;
                int needleLength = Width / 2 - 15;

                int endX = centerX + (int)(needleLength * Math.Cos(rad));
                int endY = centerY + (int)(needleLength * Math.Sin(rad));

                using (var needlePen = new Pen(_needleColor, 3))
                {
                    needlePen.EndCap = LineCap.Round;
                    g.DrawLine(needlePen, centerX, centerY, endX, endY);
                }
            }

            // Draw value text (optional - you can remove this if you prefer just the visual gauge)
            using (var font = new Font("Consolas", 7f, FontStyle.Bold))
            using (var brush = new SolidBrush(_needleColor))
            {
                var text = $"{_value:F0}%";
                var size = g.MeasureString(text, font);
                g.DrawString(text, font, brush, 
                    (Width - size.Width) / 2, 
                    Height - size.Height - 5);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }
    }
}