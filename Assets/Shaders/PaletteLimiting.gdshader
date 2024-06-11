shader_type canvas_item;
uniform sampler2D screen_texture : source_color, hint_screen_texture, filter_nearest;

uniform vec4 colors[1024]; 	// Array of max 1024 colors (should be more than enough for any pixel art palette)
uniform int num_colors = 0;	// number of colors in the palette

void fragment() {
	vec4 color = textureLod(screen_texture, SCREEN_UV, 0.0);	

	float min_color_diff = 10000.0;	
	vec4 closest_color = vec4(0.0,0.0,0.0,1.0);	
	
	for(int i = 0; i < num_colors; ++i) {
		float color_dist = distance(colors[i], color);		
		if(color_dist < min_color_diff) {
			min_color_diff = color_dist;	
			closest_color = colors[i];
		}		
	}	
	COLOR.rgb = closest_color.rgb;
}
