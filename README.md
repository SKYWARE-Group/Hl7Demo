# Hl7Demo

This app is a simple demo for parsing HL7 v2.5 messages( [OML_O21](https://hl7-definition.caristix.com/v2/HL7v2.5/TriggerEvents/OML_O21)) with [nHapi parser](https://github.com/nHapiNET/nHapi).

Demo scenario includes two messages:

* Fisrt message is for placing new order (examinations: Creatinine, Cholesterol HDL, Triglycerides, AST and ALT, [LOINC](https://loinc.org) coded)
* Second message cancels one of the examinations (Creatinine) from previously placed order (first message)

As a result, the final order have to contains only Cholesterol HDL, Triglycerides, AST and ALT.
