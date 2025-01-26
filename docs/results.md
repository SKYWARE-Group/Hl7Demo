# Results Reporting

Results reporting follows Transaction **LAB-36** (Sub-order Results Delivery) as outlined in the [IHE Inter-Laboratory Workflow](https://www.ihe.net/uploadedFiles/Documents/Laboratory/IHE_LAB_Suppl_ILW.pdf). The results are transmitted using the **ORU_R01** message (Unsolicited Transmission of an Observation Message).

## Results without previously sent order

It is important to note that **Subcontractor** can send results without having received a prior order. This situation may arise if an order is directly registered in the **Subcontractor's** LIS for any reason.  
Although according to HL7 v2.5.1 the **PATIENT** group in the ORU_R01 message is not mandatory, in the case of results without a previously sent order, it will be considered mandatory in this scenario.

The same should apply to the **VISIT** group contained within the **PATIENT** group.

## Message Structure

Depending on whether the result being sent has a previously received order (OML_O21) or not, the structure of the **ORU_R01** message is as follows:

| Group  | With prior order  | Without prior order  |
|---|---|---|
| PATIENT RESULT  | **Required**  | **Required**  |
| &nbsp;&nbsp;└─ PATIENT (PID)  | Optional  | **Required**  |
| &nbsp;&nbsp;&nbsp;&nbsp;└─ VISIT (PV1)  | Optional  | **Required**  |
| ORDER OBSERVATION (ORC, OBR) | **Required**  | **Required**  |
| &nbsp;&nbsp;└─ OBSERVATION (OBX)  | Conditional  | Conditional  |

In HL7 v2.5.1, **OBSERVATION** group within **ORDER OBSERVATION** is optional. However, it may be omitted **only in case of examination results are cancelled or unavailable**. Indeed, it doesn't make any sense to report examination item without result.

## Example messages

According to ILW, results are transmitted via **ORU_R01** - Unsolicited transmission of an observation message.

Here are a few example messages illustrating the points mentioned above. In sake of brevity, **the data in segments are bare minimum**. However, example messages **are valid** according to the specification.

### Results with previously received order

In the following example, the **Subcontractor** has received order (158524) from the **Requester** prior to performing analyses (with message OML_O21) and upon finishing the ordered examinations, it reports the results. This is the main scenario in ILW.

```hl7
MSH|^~\&|||||20250125134501||ORU^R01^ORU_R01|B1MHQY7GMMIX0RG8W039|P|2.5.1
OBR|1|158524|553684|4537-7^ESR^LN
OBX|1|NM|4537-7^ESR^LN||35|mm/h|below 15|HH|||F
OBR|2|158524|553684|24331-1^Lipid panel^LN
OBX|1|NM|2093-3^Cholesterol^LN||6.1|mmol/l|2.4-5.2|H|||F
OBX|2|NM|2571-8^Triglyceride^LN||1.6|mmol/l|0.1-1.7|N|||F
OBX|3|NM|2085-9^Cholesterol in HDL^LN||1.22|mmol/l|above 1.455|L|||F
```

Typical answer to such message would be:

```hl7
MSH|^~\&|||||20250125134525||ACK^O21^ACK|C2RTGY78HH894TF7D3E1|P|2.5.1
MSA|AA|B1MHQY7GMMIX0RG8W039|OK
```

Here we have transmitted two examinations - **ESR** and **Lipid panel**, i.e. we have two PATIENT RESULT groups (opened with OBR segments). The OBR.2 (Placer Order Number, mandatory in this scenario) referencing previously sent order and the value in OBR.2 (158524) is taken from the ORC.2.1 of the ordering OML_O21 message.

**OBR.2 is mandatory** and is critical for mapping results to correct laboratory order in **Requester's** side. If the **Requester** specify **namespace** (with ORC.2.2) it must present in the OBR segment as well.

<table>
    <thead>
        <tr>
            <th>Order ID</th>
            <th>Examination Code</th>
            <th>Examination Name</th>
            <th>Test Code</th>
            <th>Test Name</th>
            <th style="text-align: right;">Result</th>
            <th>Units</th>
            <th>Ref. range</th>
            <th>Flag</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>158524</td>
            <td>4537-7</td>
            <td>ESR</td>
            <td>4537-7</td>
            <td>ESR</td>
            <td style="text-align: right;">35</td>
            <td>mm/h</td>
            <td>below 15</td>
            <td>HH (Very high)</td>
        </tr>
        <tr>
            <td rowspan="3">158524</td>
            <td rowspan="3">24331-1</td>
            <td rowspan="3">Lipid panel</td>
            <td>2093-3</td>
            <td>Cholesterol</td>
            <td style="text-align: right;">6.1</td>
            <td>mmol/L</td>
            <td>2.4-5.2</td>
            <td>H (High)</td>
        </tr>
        <tr>
            <td>2571-8</td>
            <td>Triglyceride</td>
            <td style="text-align: right;">1.6</td>
            <td>mmol/L</td>
            <td>0.1-1.7</td>
            <td></td>
        </tr>
        <tr>
            <td>2085-9</td>
            <td>Cholesterol in HDL</td>
            <td style="text-align: right;">1.22</td>
            <td>mmol/L</td>
            <td>above 1.45</td>
            <td>L (Low)</td>
        </tr>
    </tbody>
</table>

**Note**: All examination and test codes are according to [**LOINC**](https://loinc.org/). Results are in [**SI**](https://en.wikipedia.org/wiki/International_System_of_Units) units (compatible with [UCUM](https://ucum.org/)). Flags are according to HL7 [table **0078**](https://hl7-definition.caristix.com/v2/HL7v2.5.1/Tables/0078) (Abnormal flags).

Although sending examination results from more than one order in a single message is valid, we highly recommend to avoid such practice and report only results for single order per message. Mixing results ordered in two or more orders could lead to message rejection.

### Results without previously received order

In the following example, the **Subcontractor** registered an order **with no message** from the **Requester**. It may be because order is made via phone call, on paper, etc.

Since receiving party (**Requester**) may or may not have patient's demographics, in this scenario PID and PV1 segments are mandatory. ORC segments in ORDER OBSERVATION group are highly recommended, mainly to tell the receiver who is attending doctor (ORC.12).
For simplicity, the examinations are the same as in the previous example.

```hl7
MSH|^~\&|||||20250125134501||ORU^R01^ORU_R01|B1MHQY7GMMIX0RG8W039|P|2.5.1
PID|1||8503121207^^^GRAO^NI~562387^^^LIS^MR||Ivanov^Petar||19850312|M
PV1|1|N|||||||||||||||||553684^^^LIS^MR
ORC|NW|553684||||||||||0300999977^Petrov^Ivan^^^Д-р^^^BLS^^^^DN
OBR|1||553684|4537-7^ESR^LN
OBX|1|NM|4537-7^ESR^LN||35|mm/h|below 15|HH|||F
ORC|NW|553684||||||||||0300999977^Petrov^Ivan^^^Д-р^^^BLS^^^^DN
OBR|2||553684|24331-1^Lipid panel^LN
OBX|1|NM|2093-3^Cholesterol^LN||6.1|mmol/l|2.4-5.2|H|||F
OBX|2|NM|2571-8^Triglyceride^LN||1.6|mmol/l|0.1-1.7|N|||F
OBX|3|NM|2085-9^Cholesterol in HDL^LN||1.22|mmol/l|above 1.455|L|||F
```