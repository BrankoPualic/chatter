declare global {
  interface File {
    isVideo(): boolean;
    isImage(): boolean;
  }
}

function isVideo(this: File): boolean {
  return this.type.startsWith('video/')
}

function isImage(this: File): boolean {
  return this.type.startsWith('image/');
}

File.prototype.isVideo = isVideo;
File.prototype.isImage = isImage;

export { };