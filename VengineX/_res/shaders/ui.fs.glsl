#version 450 core

uniform sampler2D uTexture;
uniform vec4 uTint;

in vec2 uv;
layout(location = 0) out vec4 fColor;

void main()
{
	vec4 sampled = texture(uTexture, uv);
	vec3 color = clamp(sampled.rgb + uTint.rgb, 0.0, 1.0);
	fColor = vec4(color, sampled.a * uTint.a);
//	fColor = uTint;
}