import smtplib
from email.MIMEMultipart import MIMEMultipart
from email.MIMEText import MIMEText
from email.MIMEBase import MIMEBase
from email import encoders
import time

class MailSender():
    def __init__(self, credentials_path='', to=''):
        self.user, self.password = self.get_credentials(credentials_path)
        self.From = self.user
        self.To = to
        self.filename = 'output.png'
        self.text = self.buildMail()
        self.sendMail()


    def get_credentials(self, credentials_path):
        with open('../../Credentials.txt', 'r') as f:
            credentials = f.read()

        split_cre = credentials.split(":")
        gmail_user = split_cre[0].strip()
        password = split_cre[1].strip()
        return gmail_user, password

    def buildMail(self):
        msg = MIMEMultipart()
        msg['From'] = self.user
        msg['To'] = self.To
        msg['Subject'] = 'PlantIO warning'
        body = "Water your Plant!"
        msg.attach(MIMEText(body, 'plain'))

        attachment = open(self.filename, "rb")

        part = MIMEBase('application', 'octet-stream')
        part.set_payload((attachment).read())
        encoders.encode_base64(part)
        part.add_header('Content-Disposition', "attachment; filename= %s" % self.filename)
        msg.attach(part)

        text = msg.as_string()
        return text

    def sendMail(self):
        try:
            server = smtplib.SMTP('smtp.gmail.com', 587)
            server.starttls()
            server.login(self.user, self.password)
            server.sendmail(self.user, self.To, self.text)
            server.quit()
            print "Email sent! " + time.strftime("%H:%M:%S")
        except:
            print "Error sending mail! " + time.strftime("%H:%M:%S")
