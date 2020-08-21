

function AddRemove(phone, email, skype, info)  {

    //Пременные для контроля числа input'ов. Минимальное число input'ов каждого типа равно одному
    this.numberOfPhones = phone;
    this.numberOfEmails = email;
    this.numberOfSkypes = skype;
    this.numberOfAddInfs = info;
    

    //Добавление input'а
    this.dynamicElement=function(element) {

        var name;
        var td = document.createElement("td");
        //Ограничение на добавление доп. элементов
        var maxNumberOfElems = 4;
        switch (element) {
            case "Phone":
                if (this.numberOfPhones > maxNumberOfElems) {
                    return
                }
                else {
                    this.numberOfPhones++;
                    td.id = "phonenonfirst" + this.numberOfPhones;
                    name = "Phone[" + this.numberOfPhones + "].PhoneNumber";
                    break;
                }
                
            case "Email":
                if (this.numberOfEmails > maxNumberOfElems) {
                    return
                }
                else {
                    this.numberOfEmails++;
                    td.id = "emailnonfirst" + this.numberOfEmails;
                    name = "Email[" + this.numberOfEmails + "].EmailAddres";
                    break;
                }
               
            case "Skype":
                if (this.numberOfSkypes > maxNumberOfElems) {
                    return
                }
                else {
                    this.numberOfSkypes++;
                    td.id = "skypenonfirst " + this.numberOfSkypes;
                    name = "Skype[" + this.numberOfSkypes + "].SkypeLogin";
                    break;
                }
               
            case "Addinf":
                if (this.numberOfAddInfs > maxNumberOfElems) {
                    return
                }
                else {
                    this.numberOfAddInfs++;
                    td.id = "addinfnonfirst " + this.numberOfAddInfs;
                    name = "Addinf[" + this.numberOfAddInfs + "].AdditionalInfo";
                    break;
                }
        }
            
        td.innerHTML = '<input type="text" name=' + name + ' />';
        document.getElementById(element).appendChild(td)
      
    }

   //Удаление input'а
    this.remove = function (element) {

        //Если id последнего input'а равно id первого input'а, то выходим из метода, иначе убираем последний input
        var tr = document.getElementById(element);
        switch (element) {

            case "Phone":
                if (tr.lastElementChild.id === tr.firstElementChild.id) {
                    this.numberOfPhones = 0;
                    return;
                }
                else {
                    this.numberOfPhones--;
                    tr.removeChild(tr.lastElementChild);
                    break;
                }

            case "Email":
                if (tr.lastElementChild.id === tr.firstElementChild.id) {
                    this.numberOfEmails = 0;
                    return;
                }
                else {
                    this.numberOfEmails--;
                    tr.removeChild(tr.lastElementChild);
                    break;
                }

            case "Skype":
                if (tr.lastElementChild.id === tr.firstElementChild.id) {
                    this.numberOfSkypes = 0;
                    return;
                }
                else {
                    this.numberOfSkypes--;
                    tr.removeChild(tr.lastElementChild);
                    break;
                }

            case "Addinf":
                if (tr.lastElementChild.id === tr.firstElementChild.id) {
                    this.numberOfAddInfs = 0;
                    return;
                }
                else {
                    this.numberOfAddInfs--;
                    tr.removeChild(tr.lastElementChild);
                    break;
                }
        }
    }    
    
}






