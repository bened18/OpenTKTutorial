#version 330 core
in vec3 aPosition;

void main()
{
    gl_Position = vec4(aPosition, 1.0); // see how we directly give a vec3 to vec4's constructor
}