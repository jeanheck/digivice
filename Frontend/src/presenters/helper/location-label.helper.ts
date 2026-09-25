type LocationLabelDelimiterRule = {
  delimiter: string;
  includeDelimiterInFirstLine: boolean;
};

const LOCATION_LABEL_DELIMITER_RULES: LocationLabelDelimiterRule[] = [
  { delimiter: ": ", includeDelimiterInFirstLine: true },
  { delimiter: " - ", includeDelimiterInFirstLine: false },
  { delimiter: " (", includeDelimiterInFirstLine: false },
  { delimiter: " / ", includeDelimiterInFirstLine: false },
];

export function splitLocationLabel(label: string): string[] {
  for (const rule of LOCATION_LABEL_DELIMITER_RULES) {
    const delimiterIndex = label.indexOf(rule.delimiter);
    if (delimiterIndex === -1) {
      continue;
    }

    const firstLine = rule.includeDelimiterInFirstLine
      ? label.slice(0, delimiterIndex + 1).trimEnd()
      : label.slice(0, delimiterIndex).trim();
    const secondLine = rule.includeDelimiterInFirstLine
      ? label.slice(delimiterIndex + rule.delimiter.length).trim()
      : label.slice(delimiterIndex).trim();

    if (secondLine === "") {
      continue;
    }

    return [firstLine, secondLine];
  }

  return [label];
}
