export const ExternalLink = {
  koFi: "https://ko-fi.com/morrodaantena",
  livePix: "https://livepix.gg/morrodaantena",
} as const;

export type ExternalLink = (typeof ExternalLink)[keyof typeof ExternalLink];
