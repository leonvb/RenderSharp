#version 400
out vec4 frag_colour;

in vec4 vertexColor;

// uniform vec4 ourColor;

void main() 
{
	frag_colour = vertexColor;
}