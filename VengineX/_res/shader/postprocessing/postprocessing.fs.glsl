#version 460

out vec4 fColor;
  
in vec2 uv;

uniform sampler2D screenTexture;

void main()
{ 
    fColor = texture(screenTexture, uv);
}