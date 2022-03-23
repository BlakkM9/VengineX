#version 460

uniform sampler2D uTexture;

in VertexOut
{
	vec4 color;
	vec2 uv;
} i;


layout (location = 0) out vec4 fColor;


void main()
{
	fColor = texture(uTexture, i.uv) * i.color;
}