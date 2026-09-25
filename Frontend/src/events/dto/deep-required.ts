export type DeepRequired<T> = T extends (infer Item)[]
  ? DeepRequired<Item>[]
  : T extends object
    ? { [Key in keyof T]-?: DeepRequired<T[Key]> }
    : T;
