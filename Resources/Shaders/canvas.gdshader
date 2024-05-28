shader_type canvas_item;

uniform vec2 canvasSize;
uniform sampler2D particleData;
uniform sampler2D particleColors;

void fragment() 
{	
	float aspectRatio = canvasSize.y / canvasSize.x;
	vec2 uv = (UV - vec2(0.5)) / vec2(aspectRatio, 1.);
	vec3 col = vec3(0.0);
	int particles = textureSize(particleData, 0).x;
	
	for (int x=0; x < particles; x++){
		vec2 texturePos = vec2(float(x) / float(particles), 0.0);
		vec4 data = texture(particleData, texturePos);
		vec2 offset = vec2(0.5 / float(particles), 0.0);
		vec3 color = texture(particleColors, texturePos + offset).rgb;
		vec2 position = data.rg;
		float size = data.b;
		
		float dy = uv.y - (position - vec2(0.5)).y;
		float dx = uv.x - (position - vec2(0.5)).x;
		
		float distSquared = dy*dy + dx*dx;
		if (distSquared <= size*size){
			col = color;
			break;
		}
	}
	
	COLOR = vec4(col, 1.0);
}