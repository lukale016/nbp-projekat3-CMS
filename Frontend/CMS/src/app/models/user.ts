
export class User {
    public name : string;
    public surname : string;
    public username : string;
    public password : string;
    public rootDir : string;
   

    constructor(Name :string, Surname :string, Username :string, Password: string, RootDir: string){
        this.name = Name;
        this.surname = Surname;
        this.username = Username;
        this.password = Password;
        this.rootDir = RootDir;
    }
}
