import { FolderItem } from "./folderItem";

export class Folder {
    public path : string;
    public children : Array<FolderItem>;

    constructor(Name :string, Children :Array<FolderItem>){
        this.path = Name;
        this.children = Children;
    }
}
