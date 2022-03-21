﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering.Shaders;
using VengineX.Graphics.Rendering.Textures;
using VengineX.Resources;
using VengineX.UI.Serialization;

namespace VengineX.UI.Elements
{
    /// <summary>
    /// Class representing an image on the ui.<br/>
    /// This is pretty much always used when there is something to render on the ui.
    /// </summary>
    public class Image : Element
    {
        /// <summary>
        /// Shader used for ui images.
        /// </summary>
        public static Shader? ImageShader { get; private set; }

        /// <summary>
        /// Location of the projection matrix uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static int ImageProjectionMatrixLocation { get; private set; }

        /// <summary>
        /// Location of the model matrix uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static int ImageModelMatrixLocation { get; private set; }

        /// <summary>
        /// Location of the view matrix uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static int ImageViewMatrixLocation { get; private set; }

        /// <summary>
        /// Location of the uTint uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static int TintLocation { get; private set; }


        public static Shader? ColorShader { get; private set; }

        /// <summary>
        /// Location of the projection matrix uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static int ColorProjectionMatrixLocation { get; private set; }

        /// <summary>
        /// Location of the model matrix uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static int ColorModelMatrixLocation { get; private set; }

        /// <summary>
        /// Location of the view matrix uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static int ColorViewMatrixLocation { get; private set; }

        /// <summary>
        /// Location of the uTint uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static int ColorLocation { get; private set; }


        /// <summary>
        /// The color that is used if there is no texture.<br/>
        /// This should be <see cref="Vector4.Zero"/> if there is any texture present.<br/>
        /// If you want to tint the texture use <see cref="Tint"/> instead.
        /// </summary>
        public Vector4 Color { get => _color; set => _color = value; }
        private Vector4 _color = Vector4.Zero;

        /// <summary>
        /// Tint of this image.
        /// </summary>
        public Vector4 Tint { get => _tint; set => _tint = value; }
        private Vector4 _tint = Vector4.One;

        /// <summary>
        /// The texture of this image.
        /// </summary>
        public Texture2D? Texture { get; set; } = null;


        /// <summary>
        /// Constructor for creating instance with <see cref="UISerializer"/>.
        /// </summary>
        public Image(Element parent) : base(parent)
        {
            // Lazy shader initialization
            if (ImageShader == null)
            {
                ImageShader = ResourceManager.GetResource<Shader>("shader.ui.image");
                ColorShader = ResourceManager.GetResource<Shader>("shader.ui.color");

                ImageProjectionMatrixLocation = ImageShader.GetUniformLocation("P");
                ImageModelMatrixLocation = ImageShader.GetUniformLocation("M");
                ImageViewMatrixLocation = ImageShader.GetUniformLocation("V");
                ColorProjectionMatrixLocation = ColorShader.GetUniformLocation("P");
                ColorModelMatrixLocation = ColorShader.GetUniformLocation("M");
                ColorViewMatrixLocation = ColorShader.GetUniformLocation("V");

                TintLocation = ImageShader.GetUniformLocation("uTint");
                ColorLocation = ColorShader.GetUniformLocation("uColor");
            }
        }

        /// <summary>
        /// Creates a new image ui element from given parameters.
        /// </summary>
        public Image(Element parent, Texture2D? texture, Vector4 color, Vector4 tint)
            : this(parent)
        {
            Color = color;
            Tint = tint;
            Texture = texture;
        }

        /// <summary>
        /// Overload for <see cref="Image(Element, Texture2D, Vector4, Vector4)"/>.
        /// </summary>
        public Image(Element parent, Vector4 color)
            : this(parent, null, color, new Vector4(1, 1, 1, 0)) { }

        /// <summary>
        /// Overload for <see cref="Image(Element, Texture2D, Vector4, Vector4)"/>.
        /// </summary>
        public Image(Element parent, Texture2D texture, Vector4 tint)
            : this(parent, texture, Vector4.Zero, tint) { }

        /// <summary>
        /// Overload for <see cref="Image(Element, Texture2D, Vector4, Vector4)"/>.
        /// </summary>
        public Image(Element parent, Texture2D texture)
            : this(parent, texture, Vector4.Zero, Vector4.One) { }


        // TODO batch rendering with instanced quads if same texture is used
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Render()
        {
            if (Visible)
            {
                CalculateModelMatrix(0);

                // Render self
                if (Texture == null)
                {
                    ColorShader.Bind();
                    ColorShader.SetUniformMat4(ColorProjectionMatrixLocation, ref ParentCanvas.ProjectionMatrix);
                    ColorShader.SetUniformMat4(ColorViewMatrixLocation, ref ParentCanvas.ViewMatrix);
                    ColorShader.SetUniformMat4(ColorModelMatrixLocation, ref ModelMatrix);
                    ColorShader.SetUniformVec4(ColorLocation, ref _color);
                }
                else
                {
                    ImageShader.Bind();
                    Texture?.Bind();
                    ImageShader.SetUniformMat4(ImageProjectionMatrixLocation, ref ParentCanvas.ProjectionMatrix);
                    ImageShader.SetUniformMat4(ImageViewMatrixLocation, ref ParentCanvas.ViewMatrix);
                    ImageShader.SetUniformMat4(ImageModelMatrixLocation, ref ModelMatrix);
                    ImageShader.SetUniformVec4(TintLocation, ref _tint);
                }

                ParentCanvas.Quad.Render();
            }


            base.Render();
        }
    }
}
