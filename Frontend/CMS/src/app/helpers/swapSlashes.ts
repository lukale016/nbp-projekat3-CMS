

export function swapSlashes(path :string) :string{
    let hasForwardSlash :boolean = path.includes('/');
    const index = path.indexOf('/');
    // let isFirstOccurence :boolean = index != -1 ? path.indexOf("_root") === index-5 : false;
    if(hasForwardSlash){
      while(hasForwardSlash){
        // if(isFirstOccurence){
            path = path.replace('/','\\\\');
            // isFirstOccurence = false;
        // }else{
            // path = path.replace('/','\\');
        // }
        console.log("replaced forwardslash path:", path);
        hasForwardSlash =  path.includes('/');
      }
      let hasMultiple = path.includes('\\\\\\\\');
      if(hasMultiple){
        while(hasMultiple){
            path = path.replace("\\\\\\\\","\\\\");
            hasMultiple = path.includes('\\\\\\\\');
        }
      }
    }else if(path.includes('\\\\')){
      let hasBackSlash = path.includes('\\\\');
      while(hasBackSlash){
        path = path.replace('\\\\','/');
        console.log("replaced backslash path:", path);
        hasBackSlash = path.includes('\\\\');
      }
    }
    return path;
  }