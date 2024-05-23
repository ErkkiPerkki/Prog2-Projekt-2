shader_type canvas_item;

uniform vec2 canvasSize;
uniform sampler2D particleData;

void fragment() 
{	
	float aspectRatio = canvasSize.y / canvasSize.x;
	vec2 uv = (UV - vec2(0.5)) / vec2(aspectRatio, 1.);
	vec3 col = vec3(0.0);
	ivec2 particles = textureSize(particleData, 0);
	
	for (int x=0; x < particles.x; x++){
		vec2 texturePos = vec2(float(x) / float(particles.x), 0.0);
		vec4 data = texture(particleData, texturePos);
		vec2 position = data.rg; //+ vec2(sin(TIME * 50. + data.r), cos(TIME * 50. + data.g)) / 100.;
		float size = data.b;
		
		float dist = distance(position - vec2(0.5), uv);
		if (dist <= size){
			col += vec3(1.0);
		}
	}
	
	COLOR = vec4(col, 1.0);
}