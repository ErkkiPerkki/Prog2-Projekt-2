shader_type canvas_item;

uniform vec2 canvasSize;
uniform sampler2D particleData;
uniform sampler2D particleColors;
uniform sampler2D particleSizes;

void fragment() 
{
	float aspectRatio = canvasSize.y / canvasSize.x;
	vec2 uv = UV / vec2(aspectRatio, 1);
	vec3 col = vec3(0.0);
	int particles = textureSize(particleData, 0).x;
	
	float particleIncrement = 1.0 / float(particles);
	float halfParticleIncrement = particleIncrement / 2.0;
	
	for (int x=0; x < particles; x++){
		vec2 texturePos = vec2(float(x) * particleIncrement, 0.0);
		vec2 offset = vec2(halfParticleIncrement, 0.0);
		vec4 data = texture(particleData, texturePos);
		vec3 color = texture(particleColors, texturePos + offset).rgb;
		float size = texture(particleSizes, texturePos + offset).r;
		
		vec2 position = data.rg / vec2(aspectRatio, 1);
		vec2 delta = uv - position;
		
		float distSquared = dot(delta, delta);
		if (distSquared <= size*size){
			col = color;
			break;
		}
	}
	
	COLOR = vec4(col, 1.0);
}