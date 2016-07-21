
function Grid1_onLoad(sender, eventArgs)
{
  // enable the 'Click to Create' button after NavBar1 has been initialized
  document.getElementById('btnGridCreate').disabled = false;
  sender.element.style.visibility = 'hidden';
}

  function buildGrid()
  {
    var person = new Object();

    person.FirstName = 'John';
    person.LastName = 'Jackson';
    person.Age = '26';
    person.Address1 = "511 King St. W";
    person.Address2 = "Suite 400";
    person.City = "Toronto";
    person.Province = "Ontario";
    person.Country = "Canada";
    person.Gender = "Male";
    person.Eyes = "Brown";
    person.Hair = "Brown";
    person.Height = "186cm";

    PopulatePropertyGrid(person);

    Grid1.element.style.visibility = 'visible';
    
  }


  function PopulatePropertyGrid(object)
  {  
    Grid1.beginUpdate(); 
    Grid1.get_table().clearData(); 

    for (var prop in object) 
    {
      var propertyName = prop
      var value = object[prop];

      if (value || value == 0 || value == '')
      {
        Grid1.get_table().addEmptyRow();
        var newRow = Grid1.get_table().getRow(Grid1.get_recordCount() - 1); 
        newRow.setValue(0, propertyName, true, true); 
        newRow.setValue(1, value, true, true); 
      }
    }

    Grid1.endUpdate(); 
  }
