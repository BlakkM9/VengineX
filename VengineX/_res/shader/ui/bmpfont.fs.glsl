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
	vec4 sampled = vec4(1.0, 1.0, 1.0, texture(uTexture, vec2(i.uv.x, i.uv.y)).r);
	fColor = i.color * sampled;
}