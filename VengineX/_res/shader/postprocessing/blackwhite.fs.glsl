#version 460

out vec4 fColor;
  
in vec2 uv;

uniform sampler2D screenTexture;

void main()
{ 
    fColor = texture(screenTexture, uv);
    float average = 0.2126 * fColor.r + 0.7152 * fColor.g + 0.0722 * fColor.b;
    fColor = vec4(average, average, average, 1.0);
}