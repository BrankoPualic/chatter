declare global {
  interface String {
    isEmptyGuid(): boolean;
    shorten(): string;
  }
}

function isEmptyGuid(this: string): boolean {
  const emptyGuid = '00000000-0000-0000-0000-000000000000';
  return this === emptyGuid;
}

function shorten(this: string): string {
  let text = this;
  let transform = undefined;

  if (this.length > 20) {
    let arr = text.split('').splice(0, 17);
    arr.push('...');
    transform = arr.join('');
  }

  return !transform ? text : transform;
}

String.prototype.isEmptyGuid = isEmptyGuid;
String.prototype.shorten = shorten;

export { };