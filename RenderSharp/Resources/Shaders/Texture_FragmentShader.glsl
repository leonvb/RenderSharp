#version 330 core
out vec4 frag_colour;

in vec2 TexCoord;
in vec3 Color;

uniform sampler2D texture0;
uniform sampler2D texture1;

// uniform vec4 ourColor;

void main() 
{
	// frag_colour = texture(texture0, TexCoord);

	frag_colour = mix(texture(texture0, TexCoord), texture(texture1, TexCoord), 0.2) * vec4(Color, 1.0);
}