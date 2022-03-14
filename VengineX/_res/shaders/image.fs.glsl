#version 450 core

uniform sampler2D uTexture;
uniform vec4 uColor;

in vec2 uv;
layout(location = 0) out vec4 fColor;

void main()
{
	vec4 sampled = texture(uTexture, uv);
	fColor = uColor * sampled;
}