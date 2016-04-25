uniform sampler2D fbo_texture;
varying vec2 f_texcoord;

void main(void) {
  vec4 color;
  color = texture2D(fbo_texture, f_texcoord);

  gl_FragColor = vec4(1.0 - color.r, 1.0 - color.g, 1.0 - color.b, color.a);
}