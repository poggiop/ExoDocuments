

  var listOfColors = ["#d5e1df","#e3eaa7", "#b5e7a0", "#86af49"];      // List of colors for border encapsulation                                            // The width of the column
  var bufferCellWidth = 20;
  var cellHeight = 25;
  var numOfRows = 5;
  var numOfCells = 4;
  var maxColumnSpan = 1;
  var nameCol;
  let colData = [{colName:'Data Elements', colSize:600},
                {colName:'Range', colSize:200},
                {colName:'Units', colSize:200},
                {colName:'Format', colSize:200},
                {colName:'Description', colSize:200}];



// Main table generation funciton. TODO: Refactor closing cells  
function generateTable(parsedData, idDiv){
    let tableData = parsedData.tableData;
    let body = document.getElementById(idDiv);
    var tbl = document.createElement("table");
    var tblBody = document.createElement("tbody");
    // Appending header row
    tblBody.appendChild(getHeaderRow(colData,parsedData.maxDepth,'LightGrey'));
    for(var i = 0; i < tableData.length; i++){
      let row ;
      console.log('|' + tableData[i].name + '|' + tableData[i].type + '||');
      row = createBufferCells(tableData[i].depth);
      row = appendNameCell(row,parsedData,i);
      row = createTailCells(row,tableData[i]);
      tblBody.appendChild(row);
      if(i+1 < tableData.length){
        if(tableData[i+1].depth < tableData[i].depth){
          for(var j = tableData[i].depth-1 ; j >=  tableData[i + 1].depth; j--){
            row = createBufferCells(j);
            row = appendClosingCells(row,parsedData.maxDepth, j);
            tblBody.appendChild(row);

          }
        }
      }
      else{
            for(var j = tableData[i].depth-1 ; j >= 0; j--){
            row = createBufferCells(j);
            row = appendClosingCells(row,parsedData.maxDepth, j);     
            tblBody.appendChild(row);
          }
      }
      
    }

  tbl.appendChild(tblBody);
  body.appendChild(tbl);
  tbl.setAttribute("border", "2");
  tbl.setAttribute("border-collapse", "collapse");
}

// HELPER FUNCTIONS
function getHeaderRow(colData,maxDepth,bgColor){
    let row = document.createElement("tr");

    for(let i = 0; i < colData.length; i++){
         let tableHeader = document.createElement('th');
        // Save name column reference
        tableHeader.colSpan = "1";
        if( i === 0){
           tableHeader.id = 'nameCol';
           console.log('maxDepth: ', maxDepth);
           tableHeader.colSpan = maxDepth + 1;
        }

        tableHeader.style.width = colData[i].colSize + 'px';
        tableHeader.textContent = colData[i].colName;
        row.appendChild(tableHeader);
        row.style.backgroundColor = bgColor;
    }
    return row;
}

function createBufferCells(depth){
  let row = document.createElement("tr");
  let width = bufferCellWidth;
  for(let j = 0; j < depth; j++){
    let cell = document.createElement("td");
    cell.style.width= width +"px";
    cell.style.backgroundColor = listOfColors[j % listOfColors.length];
    row.appendChild(cell);
  }
  return row;
}

function isLastLeaf(i, tableData){
  if(i+1 < tableData.length){
    return tableData[i].isLeaf && !tableData[i+1].isLeaf;
    }
    return tableData[i].isLeaf;
  }

function appendClosingCells(row,maxDepth, depth){
  let cell = document.createElement("td");
   cell.style.backgroundColor = listOfColors[depth % listOfColors.length];
   cell.textContent = 'End ' ;
   cell.colSpan = maxDepth+1 - depth;
   row.appendChild(cell);

     for(var j = 1; j < colData.length; j++){
      let cell = document.createElement("td");
      cell.style.backgroundColor = listOfColors[depth % listOfColors.length];
      cell.colSpan = 1;
      row.appendChild(cell);
      }
   return row;
}
function appendNameCell(row, parsedData, i){
   let cell = document.createElement("td");
   let rowData =  parsedData.tableData[i]
   cell.style.backgroundColor = listOfColors[rowData.depth % listOfColors.length];
   cell.textContent = 'Start ' + rowData.name;

   if(rowData.isLeaf){
      cell.textContent = rowData.name;
      cell.style.backgroundColor = 'white';
   }
   
   cell.colSpan = parsedData.maxDepth+1 - rowData.depth;
   row.appendChild(cell);
   return row;
}

function createTailCells(row,tableData){
  for(var j = 1; j < colData.length; j++){
      let cell = document.createElement("td");
      cell.style.backgroundColor = listOfColors[tableData.depth % listOfColors.length];
      if(tableData.isLeaf){cell.style.backgroundColor = 'white';}
      if(colData[j].colName == 'Format'){
        cell.textContent = tableData.type;
      }
      cell.colSpan = 1;
      row.appendChild(cell);
  }
    return row;
}


