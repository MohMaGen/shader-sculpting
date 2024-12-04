#version 330

in vec2 fragTexCoord;
in vec4 fragColor;
out vec4 finalColor;

uniform vec4  fogColor;
uniform float fogIntensity;
uniform float time;


float sdfSphere(vec3 pos, vec3 center, float radius)
{
    return length(pos - center) - radius;
}


float sdfCube(vec3 pos, vec3 center, float a)
{
    vec3 d = abs(pos - center) - a;
    return min(max(d.x, max(d.y, d.z)), 0) + length(max(d,0));
}


float map(vec3 pos)
{
    float dist = 0;


    dist = sdfSphere(pos, vec3(0), 0.4);
    dist = max(dist, -sdfSphere(pos, vec3(0), 0.35));
    dist = max(dist, sdfCube(pos, vec3(0), 0.3));

    return max(0, dist);
}

float fogFactor(float t) {
    return clamp(1 / exp(t * t * fogIntensity), 0.2, 1);
}


vec3 rot_y(vec3 v, float a)
{
    return v * mat3(cos(a), 0, sin(a), 0, 1, 0, -sin(a), 0, cos(a));
}

vec3 proj(vec3 a, vec3 n)
{
    return dot(a, n) * n;
}

vec3 projpl(vec3 a, vec3 n)
{
	return a + proj(a, n);
}

mat4 look_at_matrix(vec3 pos, vec3 target, vec3 up)
{
    vec3 zaxis = normalize(pos - target);
    vec3 xaxis = normalize(cross(up, zaxis));
    vec3 yaxis = cross(zaxis, xaxis);

    return mat4(
        xaxis.x, yaxis.x, zaxis.x, pos.x,
        xaxis.y, yaxis.y, zaxis.y, pos.y,
        xaxis.z, yaxis.z, zaxis.z, pos.z,
        0, 0, 0, 1
    );
}
mat3 roty(float a)
{
    return mat3(
         cos(a), 0, sin(a),
         0,      1, 0,
        -sin(a), 0, cos(a)
    );
}


void main() {
    vec2 uv = fragTexCoord - 0.5;
    vec3 camera_px = vec3(uv.xy, - 1);

    vec3 look_at  = vec3(0);
    vec3 view_pos = vec3(2, -2, 0);
    view_pos     *= roty(time);
    vec3 forward  = normalize(look_at-view_pos);

    mat4 look_at_matrix = look_at_matrix(view_pos, look_at, vec3(0,1,0));

    vec3 ray_origin    = (vec4(camera_px, 1) * look_at_matrix).xyz;
    vec3 ray_direction = ray_origin - view_pos;

    float t = 0;
    float dist; vec3 pos;
    for (int i = 0; i < 100; ++i) {
        pos   = ray_origin + ray_direction * t;
        dist  = map(pos);
        t    += dist;
    }

    if (abs(dist) < 0.001) {
        float fog_factor = fogFactor(length(t));
        vec4 c = vec4(0);

        float color = length(pos);

        int colori = int(round(color * 20));
        colori = colori % 3;


        if (colori == 0) c = vec4(0.8, 0.5, 0.3, 1);
        if (colori == 1) c = vec4(0.3, 0.8, 0.5, 1);
        if (colori == 2) c = vec4(0.5, 0.3, 0.8, 1);

        finalColor = mix(fogColor, vec4(c.rgb, 1), fog_factor);
    } else {
        finalColor = fogColor;
    }
}
