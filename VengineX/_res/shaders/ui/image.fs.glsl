#version 460

uniform sampler2D uTexture;
uniform vec4 uColor;
uniform vec4 uTint;

in vec2 uv;
layout(location = 0) out vec4 fColor;

void main()
{
	vec4 sampled = texture(uTexture, uv);
	fColor = clamp(uTint * sampled + uColor, 0.0, 1.0);
}