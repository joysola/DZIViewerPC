using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace DST.PIMS.Framework.Controls
{
    public class ImgDX9Effect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty(nameof(Input), typeof(ImgDX9Effect), 0);
        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register(nameof(Blue), typeof(double), typeof(ImgDX9Effect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register(nameof(Green), typeof(double), typeof(ImgDX9Effect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty RedProperty = DependencyProperty.Register(nameof(Red), typeof(double), typeof(ImgDX9Effect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty BrightnessProperty = DependencyProperty.Register(nameof(Brightness), typeof(double), typeof(ImgDX9Effect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(4)));
        public static readonly DependencyProperty ContrastProperty = DependencyProperty.Register(nameof(Contrast), typeof(double), typeof(ImgDX9Effect), new UIPropertyMetadata(1.0, PixelShaderConstantCallback(5)));
        public static readonly DependencyProperty GammaProperty = DependencyProperty.Register(nameof(Gamma), typeof(double), typeof(ImgDX9Effect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(6)));
        public ImgDX9Effect()
        {
            PixelShader pixelShader = new PixelShader();
            //pixelShader.UriSource = new Uri("pack://application:,,,/DST.PIMS.Framework;component/Controls/ImgViewControls/Shader/ImgDX9Effect.ps"); // 绝对路径
            // 相对路径需要将数据写入resource中
            pixelShader.UriSource = new Uri("/DST.PIMS.Framework;component/Controls/ImgViewControls/Shader/ImgDX9Effect.ps", UriKind.Relative);
            this.PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(BlueProperty);
            UpdateShaderValue(GreenProperty);
            UpdateShaderValue(RedProperty);
            UpdateShaderValue(BrightnessProperty);
            UpdateShaderValue(ContrastProperty);
            UpdateShaderValue(GammaProperty);
        }
        public Brush Input
        {
            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }
        public double Blue
        {
            get => (double)GetValue(BlueProperty);
            set => SetValue(BlueProperty, value);
        }
        public double Green
        {
            get => (double)GetValue(GreenProperty);
            set => SetValue(GreenProperty, value);
        }
        public double Red
        {
            get => (double)GetValue(RedProperty);
            set => SetValue(RedProperty, value);
        }
        public double Brightness
        {
            get => (double)GetValue(BrightnessProperty);
            set => SetValue(BrightnessProperty, value);
        }
        public double Contrast
        {
            get => (double)GetValue(ContrastProperty);
            set => SetValue(ContrastProperty, value);
        }
        public double Gamma
        {
            get => (double)GetValue(GammaProperty);
            set => SetValue(GammaProperty, value);
        }
    }
}
