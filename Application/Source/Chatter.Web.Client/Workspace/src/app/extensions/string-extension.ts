declare global {
  interface String {
    isEmptyGuid(): boolean;
  }
}

function isEmptyGuid(this: string): boolean {
  const emptyGuid = '00000000-0000-0000-0000-000000000000';
  return this === emptyGuid;
}

String.prototype.isEmptyGuid = isEmptyGuid;

export { };