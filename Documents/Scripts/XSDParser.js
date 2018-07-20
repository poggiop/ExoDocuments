// Uncomment to use with nodejs

// var DOMParser = require('xmldom').DOMParser;
// var fs = require('fs');
// parser = new DOMParser();
// var xmlString = fs.readFileSync('/Users/insight/OneDrive/workspace/github/idd_proto/xsd/longXSD.xsd','utf8');
// var xmlDoc;
// xmlDoc = parser.parseFromString(xmlString);


// *** GLOBALS ** TODO: Can you refactor this with no globals? 
var parsedOutput = {
    maxDepth: 0,
    tableData: []
};

var filteredDepth = -1;
var maxDepth = 0;


function main(oParent,depth,typeDefDict){
      typeDefDict = getTypeDefinitions(oParent,depth,typeDefDict);
      return createTree(oParent, depth, typeDefDict);
}

function createTree(oParent, depth, typeDefDict){
    if(oParent.hasChildNodes()){
        for (var oNode = oParent.firstChild; oNode; oNode = oNode.nextSibling){
            // TODO: Should this be break or continue? Only skip the complex definitions
            if(hasNameandTag(oNode,'complexType')){continue;}
            // TODO: Curry this node expansion process
            // Output name, should we start to expand node?
            if(hasNameandTag(oNode,'element')){
                filteredDepth +=1;
                maxDepth = Math.max(maxDepth,filteredDepth);
                let key = getKeyIfExists(oNode,typeDefDict);
                let keyExists = key !== null;
                let tableDataNode = {
                    'name': oNode.getAttribute('name'),
                    'depth':filteredDepth,
                    'isLeaf':!(oNode.hasChildNodes() || keyExists),
                    'tagName': oNode.tagName,
                    'type':  oNode.hasAttribute('type') ? oNode.getAttribute('type'): null,
                };

                parsedOutput.tableData.push(tableDataNode);
                //console.log(Array(depth*2).join(' ') + oNode.getAttribute('name') + ' depth: ', filteredDepth);
                // Expand node if definition is found
                if(keyExists){
                    createTree( typeDefDict[key], depth + 1, typeDefDict);
                }

            }
             createTree(oNode, depth + 1, typeDefDict);
             if(hasNameandTag(oNode,'element')){ filteredDepth -=1;}
        } // End of for loop
    }

    if(depth === 0){
        parsedOutput.maxDepth = maxDepth;
        return parsedOutput;
    }
}

function getKeyIfExists(oNode,dict){
    if(oNode.hasAttribute('type')){
        let key = formatToKey(oNode.getAttribute('type'));
        if(key in dict){return key;}
    }
    return null;
}

// TODO: cant  you just skip the branch if the root node is not complex?? Read up on complex type defintions more
function getTypeDefinitions(oParent, depth, typeDefDict){
    // Add complexType definition node to the dictionary
    if(hasNameandTag(oParent,'complexType')){
        typeDefDict[oParent.getAttribute('name')] = oParent;
    }
    if(oParent.hasChildNodes()){
        for (var oNode = oParent.firstChild; oNode; oNode = oNode.nextSibling){
            getTypeDefinitions(oNode, depth + 1, typeDefDict);
        }
    }
    if(depth === 0){ return typeDefDict; }
}
// ************************************************** HELPER FUNCTIONS **************************************************
// ************************* FORMATTERS *************************

function formatToKey(typeString){
    if(typeString.includes(':')){ return typeString.split(':')[1];}
    return typeString;
}
// ************************* BOOLEAN *************************
function dictIsEmpty(dict){
    return Object.keys(dict).length === 0;
}

function hasNameandTag(node,tagName){
    if (node.nodeType === 1){
        return node.tagName.split(':')[1] === tagName &&
        node.hasAttribute('name');
    }
    return false;
}

