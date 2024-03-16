export default interface HistoryLogProp {
  imgAvatar: any;
  sName: string;
  sPosition?: string;
  sComment?: string;
  sStatus: string;
  sActionMode?: "Draft" | "Submit" | "Approve" | "Reject" | "Cancel" | "Recall" | "Completed" | "Save Result" | "Closed";
  sActionDate: string;

}
